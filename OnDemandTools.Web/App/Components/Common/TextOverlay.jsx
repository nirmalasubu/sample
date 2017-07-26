import React from 'react';
import $ from 'jquery';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';


class TextOverlay extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        if(this.props.data!=null && this.props.data.length > this.props.numberOfChar){
            const popoverDescLeft = (
                <Popover id="popover-positioned-left">
                    <div class="TitleOverlay-height"> {this.props.data} </div>
                </Popover>
            );

            return(
                <OverlayTrigger trigger={['hover']} rootClose placement="bottom" overlay={popoverDescLeft}>
                    <div className="cursorPointer">
                    {this.props.data.substring(0,this.props.numberOfChar)} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>
            );
        }        
        else{
            return (<div>{this.props.data}</div>);
        }
    }
}

export default TextOverlay;