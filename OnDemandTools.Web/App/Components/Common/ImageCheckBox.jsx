import React from 'react';

class ImageCheckBox extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <img className={this.props.selected ? "imageCheckbox imageSelected" : "imageCheckbox" } 
            onClick={this.props.handleBrandChange.bind(this, this.props.brandName)} 
            src={"../images/brands/" + this.props.brandName + ".gif"} 
            alt={this.props.brandName}
            title={this.props.brandName}
            key={this.props.brandName}
            disabled={this.props.isDisabled?true:false}

            />
        )
    }
}

export default ImageCheckBox