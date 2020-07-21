import * as React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

const Home = () => (

  <div>
    <h1>Thornless</h1>
    <p>
      A project that I'm slowly (very slowly) working on to help with
      generating character names for Pathfinder, Dungeons &amp; Dragons,
      and other role-playing or fantasy settings.
    </p>
    <p>
      <NavLink tag={Link} className="text-dark" to="/characternames">Character Names</NavLink>
    </p>
  </div>
);

export default connect()(Home);
