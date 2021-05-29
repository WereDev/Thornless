import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as LayoutStore from '../store/layoutService';

import './Layout.scss'
import './Generator.scss'

type LayoutProps =
    LayoutStore.LayoutState
    & typeof LayoutStore.actionCreators

class FetchData extends React.Component<LayoutProps> {
    public componentDidMount() {
        this.props.requestLastUpdate()
    }

    public render() {
        return (
            <React.Fragment>
                <div id="main" className="container container-fluid flex-magic">
                    <div id="header" className="row">
                        <div id="title">
                            <a href="/">Thornless</a>
                        </div>
                        <div id="nav-menu">
                        </div>
                    </div>
                    <div id="content" className="row flex-magic">
                        {this.props.children}
                    </div>
                    <div id="footer" className="row p-1">
                        <div className="text-left w-50">
                            &copy; 2020-2021&nbsp;<a href="//weredev.com" target="_blank" rel="noopener noreferrer">Weredev</a>
                        </div>
                        <div className="text-right w-50">
                            {this.props.lastUpdate?.latestBuildDateString ?? ''}
                        </div>
                    </div>
                </div>
            </React.Fragment>
        )
    }

}


export default connect(
    (state: ApplicationState) => state.layoutState, // Selects which state properties are merged into the component's props
    LayoutStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
