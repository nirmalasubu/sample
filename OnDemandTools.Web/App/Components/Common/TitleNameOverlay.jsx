import React from 'react';
import { connect } from 'react-redux';
import { searchByTitleIds } from 'Actions/TitleSearch/TitleSearchActions';
import { Popover, OverlayTrigger, Button } from 'react-bootstrap';
@connect((store) => {
    return {
    };
})
class TitleNameOverlay extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            titleIds: [],
            titles: [],
            isProcessing: false
        }
    }

    fetchAndUpdateTitles() {

        var titleIds = this.props.data;
        this.setState({
            titlesIds: this.props.data

        });
        let searchPromise = searchByTitleIds(titleIds);

        searchPromise.then(message => {
            this.setState({ titles: message, isProcessing: false });
        })
        searchPromise.catch(error => {
            console.error(error);
            this.setState({ titles: [], isProcessing: false });
        });
    }

    componentDidMount() {
        this.setState({
            titlesIds: this.props.data,
            isProcessing: true
        });

        this.fetchAndUpdateTitles();
    }

    ///<summary>
    /// after component is mounted . update the stae when  titles are added  or removed
    ///</summary>
    compareAndUpdatetitles() {
        if (this.state.titlesIds != undefined) {
            if (this.state.titlesIds.length != this.props.data.length) {
                this.fetchAndUpdateTitles();
            } else {
                for (var i = 0; i < this.props.data.length; i++) {
                    if (this.state.titlesIds[i] != this.props.data[i]) {

                        this.fetchAndUpdateTitles();
                        return false;
                    }
                }
            }
        }
        return true;
    }


    render() {

        var titleNames = [];
        var sortedTitleRows = [];
        var titleText = "";
        this.compareAndUpdatetitles();
        if (this.state.titles.length > 1) {
            for (var i = 0; i < this.state.titles.length; i++) {
                titleNames.push(this.state.titles[i].titleName)
            }
        }
        // sort the title names
        if (titleNames.length > 1) {
            titleNames.sort();
            titleText = titleNames[0];
            for (var i = 0; i < titleNames.length; i++) {
                sortedTitleRows.push(<p key={i.toString()}> {titleNames[i]} </p>)
            }
        }

        const popoverLeft = (
            <Popover id="popover-positioned-left" title="Titles/Series">
                <div class="TitleOverlay-height"> {sortedTitleRows} </div>
            </Popover>
        );

        if (this.state.isProcessing) {
            return <div>Loading...</div>
        }
        else if (this.state.titles.length == 0) {
            return <div></div>
        }
        else if (this.state.titles.length == 1 || this.props.disableOverlay) {
            return (
                <div>
                    {titleText}
                </div>
            )
        }
        else {
            return (
                <OverlayTrigger trigger={['click']} rootClose placement="left" overlay={popoverLeft}>
                    <div title="click to view more Title/Series">
                        {titleText} <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>)
        }
    }
}

export default TitleNameOverlay