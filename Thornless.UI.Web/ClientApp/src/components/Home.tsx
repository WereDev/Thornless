import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

import './Home.scss';

const Home = () => (
  <div className="container p-0">
    <p>
      A project that I'm slowly (very slowly) working on to help with
      generating character names for Pathfinder, Dungeons &amp; Dragons,
      and other role-playing or fantasy settings.
    </p>
    <div className="row m-0">
      <div className="col-md-4 col-12 mt-4 nav-link">
        <NavLink tag={Link} to="/characternames">
          Characters
        </NavLink>
      </div>
      <div className="col-md-4 col-12 mt-4 nav-link">
        <NavLink tag={Link} to="/buildingnames">
          Buildings
        </NavLink>
      </div>
      <div className="col-md-4 col-12 mt-4 nav-link">
        <NavLink tag={Link} to="/settlements">
          Settlements
        </NavLink>
      </div>
      <div className="col-md-4 col-12 mt-4 nav-link">
        <NavLink tag={Link} to="/about">
          About
        </NavLink>
      </div>
    </div>
  </div>
);

export default connect()(Home);
