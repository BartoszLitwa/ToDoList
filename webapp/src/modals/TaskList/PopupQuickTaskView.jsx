import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupQuickTaskView extends Component {
    constructor(props) {
        super(props);

        this.state = {
        }
    }

    render() { 
        const handleSave = async () => {
            await this.props.handleTask();
    
            this.props.toggle();
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>{"Delete \n " + this.props.taskList.title + " task list"}</ModalHeader>
                <ModalBody>
                    <form>
                    <div className='center logout-wrapper'>
                    <h1 style={{"color": "red"}}>{"Are you sure You want to delete\n " + this.props.taskList.title + " task list?"}</h1>
                </div>
                    </form>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-primary' onClick={async () => await handleSave()}>Delete</button>
                    <button className='btn btn-secondary' onClick={this.props.toggle}>Cancel</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupQuickTaskView;