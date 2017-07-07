import React from 'react';

class BrandImage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        if(this.props.isCssExpected){
            return(<div class="brand-container"><img className={"brandImageIcon"}
        src={"../images/brands/" + this.props.brandName + ".gif"}
        alt={this.props.brandName}
        title={this.props.brandName}
        key={this.props.brandName}
    /></div>)
        }
        else{
            return (<img className={"brandImageIcon"}
        src={"../images/brands/" + this.props.brandName + ".gif"}
        alt={this.props.brandName}
        title={this.props.brandName}
        key={this.props.brandName}
    />)
            }
    }
}

export default BrandImage