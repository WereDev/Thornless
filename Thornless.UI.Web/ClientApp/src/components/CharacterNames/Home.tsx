import * as React from 'react';
import { connect } from 'react-redux';
import AncestryList from './AncestryList';
import { Route } from 'react-router';

const Home = () => (
    <div id="container-home" className="container p-0 flex-magic">
        <div className="row m-0">
            <div className="col-12">
                <h1>Character Names</h1>
                <p>Start by selecting your character's ancestry.</p>
            </div>
        </div>
        <Route path='/:home?' component={AncestryList} />
    </div>
);

export default connect()(Home);
