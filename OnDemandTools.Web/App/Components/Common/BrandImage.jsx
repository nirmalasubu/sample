import React from 'react';

class BrandImage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <img className={"brandImageIcon"}
                src={"../images/brands/" + this.props.brandName + ".gif"}
                alt={this.props.brandName}
                title={this.props.brandName}
                key={this.props.brandName}
            />
        )
    }
}

export default BrandImage