import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CharacterNameState extends Common.LoadingState {
    ancestries: Ancestry[];

}

export interface Ancestry {
    code: string,
    name: string,
    sortOrder: number
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAncestriesAction {
    type: 'REQUEST_ANCESTRIES';
}

interface ReceiveAncestriesAction {
    type: 'RECEIVE_ANCESTRIES';
    ancestries: Ancestry[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAncestriesAction | ReceiveAncestriesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestAncestries: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();

        if (appState && appState.characterNames?.loadState == Common.LoadingStates.IsNotStarted) {
            fetch(`/api/charactername`)
                .then(response => response.json() as Promise<Common.ApiResponse<Ancestry[]>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ANCESTRIES', ancestries: data.data });
                });

            dispatch({ type: 'REQUEST_ANCESTRIES' });
        }
    }
};

const unloadedState: CharacterNameState = { ancestries: [], loadState: Common.LoadingStates.IsNotStarted };

export const reducer: Reducer<CharacterNameState> = (state: CharacterNameState | undefined, incomingAction: Action): CharacterNameState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_ANCESTRIES':
            return {
                ancestries: state.ancestries,
                loadState: Common.LoadingStates.IsLoading
            };
        case 'RECEIVE_ANCESTRIES':
            
            return {
                ancestries: action.ancestries,
                loadState: Common.LoadingStates.IsLoaded
            }
        default:
            return unloadedState;
    }
};
