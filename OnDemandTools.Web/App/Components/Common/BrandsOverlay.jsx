import React from 'react';
import { connect } from 'react-redux';
import BrandImage from 'Components/Common/BrandImage';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
@connect((store) => {
    return {
    };
})
class BrandsOverlay extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {

        if (this.props.data === undefined || this.props.data.length == 0) {
            return <div></div>
        }

        var allRows = [];
        var firstTwoRows = [];

        var noOfBrandsToShow=2;


        for (var i = 0; i < this.props.data.length; i++) {
            if (i < noOfBrandsToShow) {
                firstTwoRows.push(<BrandImage brandName={this.props.data[i]} key={i.toString()} />);
            }
            allRows.push(<BrandImage brandName={this.props.data[i]} key={i.toString()} />);
        }


        const popoverLeft = (
            <Popover id="popover-positioned-left" title="Brands">
                {allRows}
            </Popover>
        );

        if (this.props.data.length == 0) {
            return <div></div>
        }
        if (this.props.data.length <=noOfBrandsToShow) {
            return (
                <div>
                    {firstTwoRows}
                </div>
            )
        }
        else {
            return (
                <OverlayTrigger trigger={['hover', 'focus']} placement="left" overlay={popoverLeft}>
                    <div>
                        {firstTwoRows} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>)
        }
    }
}

export default BrandsOverlay