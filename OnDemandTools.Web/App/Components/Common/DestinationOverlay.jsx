import React from 'react';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';


class DestinationOverlay extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        if(this.props.rows.length>2){
            const popoverLeft = (
                <Popover id="popover-positioned-left" title="Destinations">
                    <div class="TitleOverlay-height"> {this.props.rows} </div>
                </Popover>
            );

            return(
                <OverlayTrigger trigger={['click', 'focus']} rootClose placement="left" overlay={popoverLeft}>
                    <div className="cursorPointer" title="click to view more destinations">
                    {this.props.rows[0]}{this.props.rows[1]} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>
                );
        }
        else{
            return (
                <div>
                    {this.props.rows}
                </div>
            );
        }
    }
}

export default DestinationOverlay