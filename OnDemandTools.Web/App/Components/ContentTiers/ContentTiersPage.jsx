import React from 'react';
import PageHeader from 'Components/Common/PageHeader';

class ContentTiers extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Content Tiers";
  }

  render() {
    return (
      <div>
        <PageHeader pageName="Content Tiers"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default ContentTiers