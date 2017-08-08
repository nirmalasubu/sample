import React from 'react';
import { Button } from 'react-bootstrap';
/// <summary>
///  Displays Array strings in a Button
/// <summary>
class TextButtons extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {

        var displayString = "";
        var buttonRows = [];
        var totalLength = 0;
        var hasOverlay = false;


        if (this.props.data != undefined && this.props.data != undefined && this.props.data.length > 0) {
            for (var i = 0; i < this.props.data.length; i++) {

                var rowData = this.props.data[i];

                if (!hasOverlay) {
                    buttonRows.push(<Button className="addMarginRight" key={i.toString()}> {rowData} </Button>);
                    totalLength += rowData.length;

                    if (this.props.data.length - 1 != i
                        && totalLength > this.props.numberOfCharToDisplay) {
                        hasOverlay = true;
                    }
                }
            }
        }

        if (hasOverlay) {
            return (
                <div className="cursorPointer">
                    {buttonRows} <i class="fa fa-ellipsis-h" title={this.props.title}></i>
                </div>
            );
        }
        else {
            return (<div className="cursorPointer">{buttonRows}</div>);
        }
    }
}

export default TextButtons;