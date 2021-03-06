import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface CharacterNameState {
    ancestries: Ancestry[];
    selectedAncestry: string | null;
    ancestryOptions: AncestryOption | null;
    selectedAncestryOption: string | null;
    characterNames: CharacterName[];
    loadState: Common.LoadingStates;
}

export interface Ancestry extends Common.NameCodeSort {

}

export interface AncestryOption {
    code: string,
    name: string,
    copyright: string,
    flavorHtml: string,
    options: Common.NameCodeSort[]
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
    requestAncestryOptions: (callback: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        if (!Common.Shared.IsNullOrEmpty(appState.characterNames?.selectedAncestry)) {
            fetch(`/api/charactername/` + appState.characterNames?.selectedAncestry)
                .then(response => response.json() as Promise<Common.ApiResponse<AncestryOption>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ANCESTRY_OPTIONS', ancestryOptions: data.data });
                    callback();
                });
            dispatch({ type: 'REQUEST_ANCESTRY_OPTIONS' });
        }
    },

    requestCharacterNames: (callback: any): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        const selectedAncestry = appState.characterNames?.selectedAncestry;
        const selectedOption = appState.characterNames?.selectedAncestryOption;
        const numberToGenerate = 1;

        if (!Common.Shared.IsNullOrEmpty(selectedAncestry) && !Common.Shared.IsNullOrEmpty(selectedOption)) {
            fetch(`/api/charactername/` + selectedAncestry + `/` + selectedOption + "?count=" + numberToGenerate)
                .then(response => response.json() as Promise<Common.ApiResponse<CharacterName[]>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_CHARACTER_NAMES', characterNames: data.data });
                    callback();
                });
            dispatch({ type: 'REQUEST_CHARACTER_NAMES' });
        }
    },

    setSelectedAncestry: (selectedAncestry: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState.characterNames && selectedAncestry !== "") {
            appState.characterNames.selectedAncestry = selectedAncestry;
        }
    },
    setSelectedOption: (selectedOption: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        if (appState.characterNames) {
            appState.characterNames.selectedAncestryOption = selectedOption;
        }
    }
};

const unloadedState: CharacterNameState =
{
    ancestries: [],
    selectedAncestry: "",
    ancestryOptions: null,
    selectedAncestryOption: "",
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
            if (action.ancestries.length === 1)
            {
                currentState.selectedAncestry = action.ancestries[0].code;
            }
            currentState.ancestries = action.ancestries;
            break;
        case 'REQUEST_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoading;
            currentState.ancestryOptions = null;
            break;
        case 'RECEIVE_ANCESTRY_OPTIONS':
            currentState.loadState = Common.LoadingStates.IsLoaded;
            if (action.ancestryOptions.options.length === 1)
            {
                currentState.selectedAncestryOption = action.ancestryOptions.options[0].code;
            }
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
