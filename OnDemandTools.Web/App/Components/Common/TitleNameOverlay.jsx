import React from 'react';

class TitleNameOverlay extends React.Component {
    constructor(props) {
        super(props);        
    }

    render() {
        return (
            <div>
                {this.props.data}
            </div>
        )
    }
}

export default TitleNameOverlay