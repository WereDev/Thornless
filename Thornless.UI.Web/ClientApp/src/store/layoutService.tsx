import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import * as Common from '../shared';

export interface LayoutState {
    lastUpdate: LastUpdate | null
}

export interface LastUpdate {
    latestBuildDate: Date | null
    latestBuildDateString: string | null
}

interface RequestLastUpdateDate {
    type: 'REQUEST_LASTUPDATED';
}

interface ReceiveLastUpdateDate {
    type: 'RECEIVE_LASTUPDATED';
    lastUpdate: LastUpdate;
}

type KnownAction = RequestLastUpdateDate
    | ReceiveLastUpdateDate;

const unloadedState: LayoutState =
{
    lastUpdate: null,
};

const months = [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Set', 'Oct', 'Nov', 'Dec']

export const reducer: Reducer<LayoutState> = (state: LayoutState | undefined, incomingAction: Action): LayoutState => {
    if (state === undefined) {
        return unloadedState;
    }

    let currentState = JSON.parse(JSON.stringify(state)) as LayoutState;
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_LASTUPDATED':
            break;
        case 'RECEIVE_LASTUPDATED':
            let date = new Date(action.lastUpdate.latestBuildDate!);
            let dateString = date?.getDate().toString().padStart(2,'0')
                             + ' '
                             + months[date?.getMonth() ?? 0]
                             + ' '
                             + date?.getFullYear();
            currentState.lastUpdate = {
                latestBuildDate: date,
                latestBuildDateString: dateString
            };
            break;
    }
    return currentState;
}; 

export const actionCreators = {
    requestLastUpdate: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();

        if (!appState?.layoutState?.lastUpdate) {
            fetch(`/api/version/lastupdate`)
                .then(response => response.json() as Promise<Common.ApiResponse<LastUpdate>>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_LASTUPDATED', lastUpdate: data.data });
                })
        }        
    }
}