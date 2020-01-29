import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface CharacterNameState {
    ancestries: Ancestry[];
    ancestryOptions: AncestryOption[];
    loadState: Common.LoadingStates;
}

export interface Ancestry {
    code: string,
    name: string,
    sortOrder: number
}

export interface AncestryOption {
    code: string,
    name: string,
    sortOrder: number
}

interface RequestAncestriesAction {
    type: 'REQUEST_ANCESTRIES';
}

interface ReceiveAncestriesAction {
    type: 'RECEIVE_ANCESTRIES';
    ancestries: Ancestry[];
}

interface RequestAncestryOptionsAction {
    type: 'REQUEST_ANCESTRY_OPTIONS';
}

interface ReceiveAncestryOptionsAction {
    type: 'RECEIVE_ANCESTRY_OPTIONS';
    ancestryOptions: AncestryOption[];
}

type KnownAction = RequestAncestriesAction
                    | ReceiveAncestriesAction
                    | RequestAncestryOptionsAction
                    | ReceiveAncestryOptionsAction;

export const actionCreators = {
    requestAncestries: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.characterNames?.loadState === Common.LoadingStates.IsNotStarted) {
            fetch(`/api/charactername`)
                .then(response => response.json() as Promise<Common.ApiResponse<Ancestry[]>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ANCESTRIES', ancestries: data.data });
                });

            dispatch({ type: 'REQUEST_ANCESTRIES' });
        }
    },
    requestAncestryOption: (selectedAncestry: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        fetch(`/api/charactername/` + selectedAncestry)
            .then(response => response.json() as Promise<Common.ApiResponse<AncestryOption[]>>)
            .then(data => {
                dispatch({ type: 'RECEIVE_ANCESTRY_OPTIONS', ancestryOptions: data.data });
            });

        dispatch({ type: 'REQUEST_ANCESTRIES' });
    }
};

const unloadedState: CharacterNameState =
{
    ancestries: [],
    ancestryOptions: [],
    loadState: Common.LoadingStates.IsNotStarted
};

export const reducer: Reducer<CharacterNameState> = (state: CharacterNameState | undefined, incomingAction: Action): CharacterNameState => {
    if (state === undefined) {
        return unloadedState;
    }

    let currentState = JSON.parse(JSON.stringify(state)) as CharacterNameState;
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_ANCESTRIES':
            currentState.loadState = Common.LoadingStates.IsLoading;
            break;
        case 'RECEIVE_ANCESTRIES':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            currentState.ancestries = action.ancestries;
            currentState.ancestryOptions = [];
            break;
        case 'REQUEST_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoading;
            currentState.ancestryOptions = [];
            break;
        case 'RECEIVE_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            currentState.ancestryOptions = action.ancestryOptions;
            break;
    }
    return currentState;
};
