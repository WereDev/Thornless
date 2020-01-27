import * as React from 'react';
import { connect } from 'react-redux';
import AncestryList from './AncestryList';
import { Route } from 'react-router';

const Home = () => (
    <div>
        <h1>Character Names</h1>
        <p>Start by selecting your character's ancestry.</p>
        <Route path='/:home?' component={AncestryList} />
    </div>
);

export default connect()(Home);
