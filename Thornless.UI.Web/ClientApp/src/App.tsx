import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import CharacterNameHome from './components/CharacterNames/Home';

import './App.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/characternames' component={CharacterNameHome} />
    </Layout>
);
