export {default as BuildingsSvg} from './buildings.svg';
export {default as CharactersSvg} from './characters.svg';
export {default as Home} from './home.svg';
export {default as Settlements} from './settlements.svg';
export {default as ThornlessSvg} from './thornless.svg';
export {default as Shared} from './shared';

export interface ApiResponse<T> {
    data: T;
};

export enum LoadingStates {
    IsLoading,
    IsLoaded,
    IsNotStarted
};
