import React from 'react';
import $ from 'jquery';
import { Popover, OverlayTrigger } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
/// <summary>
///  Displays Array strings in a Overlay
/// <summary>
class TextsOverlay extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {

        var displayString = "";
        var rows = [];
        var buttonRows = [];
        var totalLength = 0;
        var hasOverlay = false;


        if (this.props.data != undefined && this.props.data != undefined && this.props.data.length > 0) {
            for (var i = 0; i < this.props.data.length; i++) {

                var rowData = this.props.data[i];

                rows.push(<p>{rowData}</p>);

                if (!hasOverlay) {
                    buttonRows.push(<Button className="addMarginRight" key={i.toString()}> {rowData} </Button>);
                    totalLength += rowData.length;

                    if (totalLength > this.props.numberOfCharToDisplay) {
                        hasOverlay = true;
                    }
                }
            }
        }

        if (hasOverlay) {
            const popoverDescLeft = (
                <Popover id="popover-positioned-left">
                    <div class="TitleOverlay-height"> {rows} </div>
                </Popover>
            );

            return (
                <OverlayTrigger trigger={['click']} rootClose placement="bottom" overlay={popoverDescLeft}>
                    <div className="cursorPointer">
                        {buttonRows} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>
            );
        }
        else {
            return (<div>{buttonRows}</div>);
        }
    }
}

export default TextsOverlay;