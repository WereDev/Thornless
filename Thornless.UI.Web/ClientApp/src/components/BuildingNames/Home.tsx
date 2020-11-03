import * as React from 'react';
import { connect } from 'react-redux';
import BuildingList from './BuildingList';
import { Route } from 'react-router';

const Home = () => (
    <div id="container-home" className="container p-0 flex-magic">
        <div className="row m-0">
            <div className="col-12 p-0">
                <h1>Building Names</h1>
                <p>What kind of building?</p>
            </div>
        </div>
        <Route path='/:home?' component={BuildingList} />
    </div>
);

export default connect()(Home);
