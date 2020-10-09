import * as React from 'react';

import './Layout.scss'

export default (props: { children?: React.ReactNode }) => (
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
                {props.children}
            </div>
            <div id="footer" className="row pb-1">
                Copyright &copy; 2020
            </div>
        </div>
    </React.Fragment>
);
