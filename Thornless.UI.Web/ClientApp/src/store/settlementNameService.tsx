import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface SettlementState {
    settlements: SettlementType[];
    selectedSettlement: string | null;
    generatedSettlement: GeneratedSettlement | null;
    loadState: Common.LoadingStates;
}

export interface ListSettlementsResponse
{
    settlementTypes: SettlementType[];
}

export interface SettlementType extends Common.NameCodeSort {
    minSize: number,
    maxSize: number
}

export interface GeneratedSettlement
{
    code: string,
    name: string,
    population: number,
    buildingTypes: SettlementBuildingType[]
}

export interface SettlementBuildingType
{
    name: string,
    code: string,
    buildings: SettlementBuilding[]
}

export interface SettlementBuilding
{
    buildingName: string
}

interface RequestSettlementsAction {
    type: 'REQUEST_SETTLEMENTS';
}

interface ReceiveSettlementsAction {
    type: 'RECEIVE_SETTLEMENTS';
    settlements: SettlementType[];
}

interface RequestSettlementGenerationAction {
    type: 'REQUEST_SETTLEMENT_GENERATION';
}

interface ReceiveSettlementGenerationAction {
    type: 'RECEIVE_SETTLEMENT_GENERATION';
    generatedSettlement: GeneratedSettlement;
}

type KnownAction = RequestSettlementsAction
                    | ReceiveSettlementsAction
                    | RequestSettlementGenerationAction
                    | ReceiveSettlementGenerationAction;

export const actionCreators = {
    requestSettlements: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.settlements?.loadState === Common.LoadingStates.IsNotStarted) {
            fetch(`/api/settlements`)
                .then(response => response.json() as Promise<Common.ApiResponse<ListSettlementsResponse>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_SETTLEMENTS', settlements: data.data.settlementTypes });
                });

            dispatch({ type: 'REQUEST_SETTLEMENTS' });
        }
    },
    requestSettlementGeneration: (callback: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        if (!Common.Shared.IsNullOrEmpty(appState.settlements?.selectedSettlement)) {
            fetch(`/api/settlements/` + appState.settlements?.selectedSettlement)
                .then(response => response.json() as Promise<Common.ApiResponse<GeneratedSettlement>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_SETTLEMENT_GENERATION', generatedSettlement: data.data });
                    callback();
                });
            dispatch({ type: 'REQUEST_SETTLEMENT_GENERATION' });
        }
    },

    setSelectedSettlement: (selectedSettlement: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.settlements && selectedSettlement !== "") {
            appState.settlements.selectedSettlement = selectedSettlement;
        }
    },
};

const unloadedState: SettlementState =
{
    settlements: [],
    selectedSettlement: null,
    generatedSettlement: null,
    loadState: Common.LoadingStates.IsNotStarted
};

export const reducer: Reducer<SettlementState> = (state: SettlementState | undefined, incomingAction: Action): SettlementState => {
    if (state === undefined) {
        return unloadedState;
    }

    let currentState = JSON.parse(JSON.stringify(state)) as SettlementState;
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_SETTLEMENTS':
            currentState.loadState = Common.LoadingStates.IsLoading;
            break;
        case 'RECEIVE_SETTLEMENTS':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            if (action.settlements.length === 1)
            {
                currentState.selectedSettlement = action.settlements[0].code;
            }
            currentState.settlements = action.settlements;
            break;
        case 'REQUEST_SETTLEMENT_GENERATION':
            currentState.loadState = Common.LoadingStates.IsLoading;
            currentState.generatedSettlement = null;
            break;
        case 'RECEIVE_SETTLEMENT_GENERATION':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            currentState.generatedSettlement = action.generatedSettlement;
            break;
    }
    return currentState;
};
