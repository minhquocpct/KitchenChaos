using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress {
    public event EventHandler<OnStateFryingChangedEventArgs> OnStateFryingChanged;
    public event EventHandler<IHasProgress.OnProgressEventArgs> OnProgressEventChange;

    public class OnStateFryingChangedEventArgs: EventArgs {
        public StateFrying stateFrying;
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    public enum StateFrying {
        Idle,
        Frying,
        Fried,
        Burned
    }
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private StateFrying stateFrying;
    private float fryingTimer;
    private float burningTimer; 

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchen object here
            if (player.HasKitchenObject()) {
                // Player carry something
                if (HasRepiceOutput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = getFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    stateFrying = StateFrying.Frying;
                    fryingTimer = 0f;
                    if (OnStateFryingChanged != null) {
                        OnStateFryingChanged(this, new OnStateFryingChangedEventArgs {
                            stateFrying = stateFrying
                        });
                    }
                    if (OnProgressEventChange != null) {
                        OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                            progressNormalized = (float) fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                    }      
                }
            } else {
                // Player has nothing
            }
        } else {
            // There is kitchen object here
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player holding plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                        stateFrying = StateFrying.Idle;
                        if (OnStateFryingChanged != null) {
                            OnStateFryingChanged(this, new OnStateFryingChangedEventArgs {
                                stateFrying = stateFrying
                            });
                        }
                        if (OnProgressEventChange != null) {
                            OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                                progressNormalized = 0f
                            });
                        }  
                    }
                }
            } else {
                // player not carry something
                GetKitchenObject().SetKitchenObjectParent(player);
                stateFrying = StateFrying.Idle;
                if (OnStateFryingChanged != null) {
                    OnStateFryingChanged(this, new OnStateFryingChangedEventArgs {
                        stateFrying = stateFrying
                    });
                }
                if (OnProgressEventChange != null) {
                    OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                        progressNormalized = 0f
                    });
                }  
            }
        }
    }

    public override void InteractAlt(Player player) {
        // throw new System.NotImplementedException();
    }
    private void Start() {
        stateFrying = StateFrying.Idle;
    }

    private void Update() {
        if(HasKitchenObject()) {
            switch (stateFrying) {
                case StateFrying.Idle:
                    break;
                case StateFrying.Frying:
                    fryingTimer += Time.deltaTime;
                    if (OnProgressEventChange != null) {
                        OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                            progressNormalized = (float) fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                    } 
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        stateFrying = StateFrying.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = getBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        if (OnStateFryingChanged != null) {
                            OnStateFryingChanged(this, new OnStateFryingChangedEventArgs {
                                stateFrying = stateFrying
                            });
                        }
                    } 
                    break;
                case StateFrying.Fried:
                    burningTimer += Time.deltaTime;
                    if (OnProgressEventChange != null) {
                        OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                            progressNormalized = (float) burningTimer / burningRecipeSO.burningTimerMax
                        });
                    } 
                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        stateFrying = StateFrying.Burned;

                        if (OnStateFryingChanged != null) {
                            OnStateFryingChanged(this, new OnStateFryingChangedEventArgs {
                                stateFrying = stateFrying
                            });
                        }
                        if (OnProgressEventChange != null) {
                            OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                                progressNormalized = 0f
                            });
                        }      
                    } 
                    break;
                case StateFrying.Burned:
                    break;
            }
        }
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = getFryingRecipeSOWithInput(kitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else {
            return null;
        }
    }
    private bool HasRepiceOutput(KitchenObjectSO kitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = getFryingRecipeSOWithInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private FryingRecipeSO getFryingRecipeSOWithInput(KitchenObjectSO kitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == kitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO getBurningRecipeSOWithInput(KitchenObjectSO kitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == kitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
