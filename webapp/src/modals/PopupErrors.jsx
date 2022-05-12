import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupErrors extends Component {
    constructor(props) {
        super(props);
        this.state = {

        }
    }

    

    render() { 
        const toggle = () => {
            this.props.toggle();
            this.props.clearData();
        }

        const showErrors = () => {
            if(this.props.errors != null) {
                return this.props.errors.map((obj, index) => {
                    return <h3 style={{"color": "red"}} key={index}> {obj + " \n"} </h3>;
                });
            } 
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>Errors</ModalHeader>
                <ModalBody>
                    <div className=''>
                        {showErrors()}
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-secondary' onClick={toggle}>Close</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupErrors;