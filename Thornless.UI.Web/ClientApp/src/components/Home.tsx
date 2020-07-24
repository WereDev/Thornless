import * as React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CharactersSvg } from '../shared';

import './Home.scss';

const Home = () => (

  <div>
    <p>
      A project that I'm slowly (very slowly) working on to help with
      generating character names for Pathfinder, Dungeons &amp; Dragons,
      and other role-playing or fantasy settings.
    </p>
    <div className="row">
      <div className="col-md-4 col-12 mt-4 text-center">
        <NavLink tag={Link} to="/characternames">
          <CharactersSvg />
        </NavLink>
      </div>
      <div className="col-md-4 col-12 mt-4">
        {/* <BuildingsSvg /> */}
      </div>
      <div className="col-md-4 col-12 mt-4">
        {/* <Settlements /> */}
      </div>
    </div>
  </div>
);

export default connect()(Home);
