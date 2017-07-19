import React from 'react';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';


class DestinationOverlay extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        if(this.props.rows.length>this.props.numberOfDestinations){
            const popoverLeft = (
                <Popover id="popover-positioned-left" title="Destinations">
                    <div class="TitleOverlay-height"> {this.props.rows} </div>
                </Popover>
            );

            var displayRows = [];
            for(var i=0; i<this.props.numberOfDestinations; i++)
            {
                displayRows.push(this.props.rows[i]);
            }

            return(
                <OverlayTrigger trigger={['click', 'focus']} rootClose placement="left" overlay={popoverLeft}>
                    <div className="cursorPointer" title="click to view more destinations">
                    {displayRows} <i class="fa fa-ellipsis-h"></i>
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