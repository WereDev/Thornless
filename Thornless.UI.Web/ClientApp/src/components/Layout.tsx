import * as React from 'react';
import { ThornlessSvg } from '../shared/';

import './Layout.scss'

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <div id="main" className="container">
            <div className="row">
                <div className="col-md-4 col-6 mt-4 app-title">
                    <ThornlessSvg />
                </div>
            </div>
            <div className="row">
                {props.children}
            </div>
        </div>
    </React.Fragment>
);
