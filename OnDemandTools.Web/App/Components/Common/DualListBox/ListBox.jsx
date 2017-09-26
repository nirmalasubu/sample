import React from 'react';
import { Checkbox, Grid, Row, Col, InputGroup, Radio, Form, ControlLabel, FormGroup, FormControl, Button } from 'react-bootstrap';

class ListBox extends React.Component {
    constructor(props) {
        super(props);
    }   

    renderSelect() {
        const {
            children,
            controlKey,
            displayName,
            inputRef,
            onDoubleClick,
            onKeyUp,
            onSelectChange,
            disableAll
        } = this.props;

        return (
            <div>
                <label >
                    {displayName}
                </label><br/>
                <select
                    disabled={disableAll}
                    class="rdl-control"
                    id={controlKey}
                    multiple
                    ref={inputRef}
                    onDoubleClick={onDoubleClick}
                    onKeyUp={onKeyUp}
                    onChange={onSelectChange}
                >
                    {children}
                </select>
            </div>
        );
    }

    render() {
        const { canAvailableFilter, canSelectedFilter, actions, controlKey } = this.props;

        return (
            <div>
                {this.renderSelect()}
            </div>
        );        
    }
}

export default ListBox