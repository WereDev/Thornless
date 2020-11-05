import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface BuildingNameState {
    buildingTypes: BuildingType[];
    buildingNames: BuildingName[];
    selectedBuildingType: string | null;
    loadState: Common.LoadingStates;
}

export interface BuildingType extends Common.NameCodeSort {

}

export interface BuildingName {
    buildingName: string,
    buildingTypeCode: string,
    buildingTypeName: string
}


interface RequestBuldingTypesAction {
    type: 'REQUEST_BUILDING_TYPES';
}

interface ReceiveBuldingTypesAction {
    type: 'RECEIVE_BUILDING_TYPES';
    buildingTypes: BuildingType[];
}

interface RequestBuildingNamesAction {
    type: 'REQUEST_BUILDING_NAMES';
}

interface ReceiveBuildingNamesAction {
    type: 'RECEIVE_BUILDING_NAMES';
    buildingName: BuildingName;
}

type KnownAction = RequestBuildingNamesAction
                    | RequestBuldingTypesAction
                    | ReceiveBuildingNamesAction
                    | ReceiveBuldingTypesAction;

export const actionCreators = {
    requestBuildingTypes: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.buildingNames?.loadState === Common.LoadingStates.IsNotStarted) {
            fetch(`/api/buildingname`)
                .then(response => response.json() as Promise<Common.ApiResponse<BuildingType[]>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_BUILDING_TYPES', buildingTypes: data.data });
                });

            dispatch({ type: "REQUEST_BUILDING_TYPES" });
        }
    },
    requestBuildingNames: (callback: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        if (!Common.Shared.IsNullOrEmpty(appState.buildingNames?.selectedBuildingType)) {
            fetch(`/api/buildingname/` + appState.buildingNames?.selectedBuildingType)
                .then(response => response.json() as Promise<Common.ApiResponse<BuildingName>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_BUILDING_NAMES', buildingName: data.data });
                    callback();
                })
            dispatch({type: 'REQUEST_BUILDING_NAMES'});
        }
    },

    setSelectedBuildingType: (selectedBuildingTypes: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.buildingNames && selectedBuildingTypes !== "") {
            appState.buildingNames.selectedBuildingType = selectedBuildingTypes;
        }
    }
};

const unloadedState: BuildingNameState =
{
    buildingNames: [],
    buildingTypes: [],
    selectedBuildingType: "",
    loadState: Common.LoadingStates.IsNotStarted
};

export const reducer: Reducer<BuildingNameState> = (state: BuildingNameState | undefined, incomingAction: Action): BuildingNameState => {
    if (state === undefined) {
        return unloadedState;
    }

    let currentState = JSON.parse(JSON.stringify(state)) as BuildingNameState;
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_BUILDING_TYPES':
            currentState.loadState = Common.LoadingStates.IsLoading;
            break;
        case 'RECEIVE_BUILDING_TYPES':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            if (action.buildingTypes.length === 1) {
                currentState.selectedBuildingType = action.buildingTypes[0].code;
            }
            currentState.buildingTypes = action.buildingTypes;
            break;
        case 'REQUEST_BUILDING_NAMES':
            currentState.loadState = Common.LoadingStates.IsLoading;
            break;
        case 'RECEIVE_BUILDING_NAMES':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            let buildignNames = [ action.buildingName ];
            currentState.buildingNames = buildignNames.concat(currentState.buildingNames);
            break;
    }
    return currentState;
};
