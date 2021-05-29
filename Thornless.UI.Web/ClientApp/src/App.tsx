import * as React from 'react';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import CharacterNameHome from './components/CharacterNames/Home';
import BuildingNameHome from './components/BuildingNames/Home';
import SettlementHome from './components/Settlement/Home';
import AboutHome from './components/About/Home';
import NotFound from './components/404';

import './App.scss'

export default () => (
    <Layout>
        <Switch>
            <Route exact path='/' component={Home} />
            <Route exact path='/characternames' component={CharacterNameHome} />
            <Route exact path='/buildingnames' component={BuildingNameHome} />
            <Route exact path='/settlements' component={SettlementHome} />
            <Route exact path='/about' component={AboutHome} />
            <Route component={NotFound} />
        </Switch>
    </Layout>
);
