import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

// import './Home.scss';

const Home = () => (
  <div className="container p-0">
    <p>
      This is not the link you're looking for!
    </p>
  </div>
);

export default connect()(Home);
