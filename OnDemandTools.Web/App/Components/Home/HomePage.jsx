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

    var userFullName = "";

    if (this.props.user.userPermission != undefined) {
        userFullName = this.props.user.userPermission .firstName + " " + this.props.user.userPermission.lastName;
    }

    return (
      <div>
        <PageHeader pageName="Home" />
        <p>Welcome {userFullName}!</p>
      </div>
    )
  }
}

export default Home