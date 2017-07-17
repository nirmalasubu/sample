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
            <OverlayTrigger trigger={['hover', 'focus']} placement="top" overlay={popoverLeft}>
                <a href="#"><i class="glyphicon glyphicon-info-sign"></i></a>
            </OverlayTrigger>)
    }
}

export default InfoOverlay