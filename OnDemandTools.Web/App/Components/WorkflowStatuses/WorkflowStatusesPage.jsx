import React from 'react';
import PageHeader from 'Components/Common/PageHeader';
class WorkflowStatuses extends React.Component {

  constructor(props) {
    super(props);
  }
  componentDidMount() {
    document.title = "ODT - Workflow Statuses";
  }

  render() {
    return (
      <div>
        <PageHeader pageName="Workflow Statuses"/>
        <p>Under construction</p>
      </div>
    )
  }
}

export default WorkflowStatuses