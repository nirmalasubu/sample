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

    render() {

        var rows = [];

        if (this.state.titles.length > 1) {
            for (var i = 0; i < this.state.titles.length; i++) {
                rows.push(<p key={i.toString()}> {this.state.titles[i].titleName} </p>)
        }
    }

    const popoverLeft = (
        <Popover id="popover-positioned-left" title="Titles/Series">
    {rows}
        </Popover>
    );

        if (this.state.isProcessing) {
            return <div>Loading...</div>
        }
        else if (this.state.titles.length == 0) {
            return <div></div>
        }
        else if (this.state.titles.length == 1) {
            return (
                <div>
                    {this.state.titles[0].titleName}
                </div>
            )
        }
        else {
            return (                                 
                <OverlayTrigger trigger={['hover', 'focus']} placement="left" overlay={popoverLeft}>
                    <div>
                        { this.state.titles[0].titleName } <i class="fa fa-ellipsis-h"></i>
                    </div>
                </OverlayTrigger>)
                        }
                        }
    }

    export default TitleNameOverlay