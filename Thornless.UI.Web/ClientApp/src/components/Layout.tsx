import * as React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

import './Layout.scss'

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <div id="main" className="container">
            <div className="row">
                <div className="col-md-4 col-6 mt-4 app-title">
                    <NavLink tag={Link} to="/characternames">Thornless</NavLink>
                </div>
            </div>
            <div className="row">
                {props.children}
            </div>
        </div>
    </React.Fragment>
);
