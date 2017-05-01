import React from 'react';

class PageHeader extends React.Component {
    constructor(props) {
        super(props);        
    }

    render() {
        return (
            <div>
                <label className="queue-head">{this.props.pageName}</label>
                <hr />
            </div>
        )
    }
}

export default PageHeader