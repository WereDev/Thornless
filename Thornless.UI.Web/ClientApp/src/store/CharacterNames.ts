import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface NameCodeSort {
    code: string,
    name: string,
    sortOrder: number
}

export interface CharacterNameState {
    ancestries: Ancestry[];
    ancestryOptions: AncestryOption | null;
    characterNames: CharacterName[];
    loadState: Common.LoadingStates;
}

export interface Ancestry extends NameCodeSort {

}

export interface AncestryOption {
    code: string,
    name: string,
    copyright: string,
    flavorHtml: string,
    options: NameCodeSort[]
}

export interface CharacterName {
    name: string,
    ancestryCode: string,
    ancestryName: string,
    optionCode: string,
    optionName: string,
    definitions: CharacterNameDefinition[]
}

export interface CharacterNameDefinition {
    namePart: string,
    meanings: string[]
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
    ancestryOptions: AncestryOption;
}

interface RequestCharacterNamesAction {
    type: 'REQUEST_CHARACTER_NAMES';
}

interface ReceiveCharacterNamesAction {
    type: 'RECEIVE_CHARACTER_NAMES';
    characterNames: CharacterName[];
}

type KnownAction = RequestAncestriesAction
    | ReceiveAncestriesAction
    | RequestAncestryOptionsAction
    | ReceiveAncestryOptionsAction
    | RequestCharacterNamesAction
    | ReceiveCharacterNamesAction;

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
        if (selectedAncestry !== "") {
            fetch(`/api/charactername/` + selectedAncestry)
                .then(response => response.json() as Promise<Common.ApiResponse<AncestryOption>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ANCESTRY_OPTIONS', ancestryOptions: data.data });
                });
        }

        dispatch({ type: 'REQUEST_ANCESTRY_OPTIONS' });
    },
    requestCharacterNames: (selectedAncestry: string, selectedOption: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        if (selectedAncestry !== "" && selectedOption !== "") {
            fetch(`/api/charactername/` + selectedAncestry + `/` + selectedOption)
                .then(response => response.json() as Promise<Common.ApiResponse<CharacterName[]>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_CHARACTER_NAMES', characterNames: data.data });
                });
        }
    }
};

const unloadedState: CharacterNameState =
{
    ancestries: [],
    ancestryOptions: null,
    characterNames: [],
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
            action.ancestries.unshift({ code: "", name: "-- Select Ancestry --", sortOrder: -1 })
            currentState.ancestries = action.ancestries;
            break;
        case 'REQUEST_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoading;
            currentState.ancestryOptions = null;
            break;
        case 'RECEIVE_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            action.ancestryOptions.options.unshift({ code: "", name: "-- Select Option --", sortOrder: -1 })
            currentState.ancestryOptions = action.ancestryOptions;
            break;
        case 'REQUEST_CHARACTER_NAMES':
            currentState.loadState = Common.LoadingStates.IsLoading;
            break;
        case 'RECEIVE_CHARACTER_NAMES':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            currentState.characterNames = action.characterNames.concat(currentState.characterNames);
            break;
    }
    return currentState;
};
