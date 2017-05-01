import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
import { connect } from "react-redux"


@connect((store) => {
  return {
    user: store.user,
  };
})
class Home extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Home";
  }

  render() {
    return (
      <div>
        <PageHeader pageName="Home"/>
        <p>Welcome {this.props.user}!</p>
      </div>
    )
  }
}

export default Home