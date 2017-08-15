import React from 'react';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
class InfoOverlay extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const popoverLeft = (
            <Popover id="popover-positioned-left">
                {this.props.data}
            </Popover>
        );
        return (
            <OverlayTrigger trigger={['hover', 'focus']} placement={this.props.placement ? this.props.placement : "top"} overlay={popoverLeft}>
                <i class={this.props.className ? "glyphicon glyphicon-info-sign " + this.props.className : "glyphicon glyphicon-info-sign blueColorText"} ></i>
            </OverlayTrigger>)
    }
}

export default InfoOverlay