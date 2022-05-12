import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupToggleStatus extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskCompletedDate: this.props.task.completedDate,
            taskCompleted: this.props.task.isCompleted
        }
    }

    currentDate(d) {
        return `${d.getFullYear()}-${`${d.getMonth() +
            1}`.padStart(2, 0)}-${`${d.getDate()}`.padStart(
            2,
            0
          )}T${`${d.getHours()}`.padStart(
            2,
            0
          )}:${`${d.getMinutes()}`.padStart(2, 0)}`;
    }

    clearData() {
        this.setState({
            taskCompletedDate: this.currentDate(),
            taskCompleted: false,
        });
    }

    render() { 
        const handleSave = async () => {
            let taskObj = {
                Id: this.props.task.id,
                //IsCompleted: this.state.taskCompleted,
                CompletedDate: this.state.taskCompleted === true ?
                 this.currentDate(new Date(this.state.taskCompletedDate)) : null,
            };

            await this.props.handleTask(taskObj);
    
            this.props.toggle();
        }
    
        const handleChange = (e) => {
            const {name, value, checked} = e.target;
            if(name === "taskCompletedDate") {
                this.setState({taskCompletedDate: value});
            } else if(name === "taskCompleted") {
                this.setState({taskCompleted: checked});
            } 
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>Toggle Task Status</ModalHeader>
                <ModalBody>
                    <form>
                        <div className="form-group" style={{"alignItems": "stretch"}}>
                            <label>Task Completed</label>
                            <input type="checkbox" className='form-checkbox' name="taskCompleted"
                            checked={this.state.taskCompleted} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Completed Date</label>
                            <input type="datetime-local" className="form-control" name="taskCompletedDate"
                            value={this.state.taskCompletedDate} onChange={handleChange}/>
                        </div>
                    </form>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-primary' onClick={async () => await handleSave()}>Toggle</button>
                    <button className='btn btn-secondary' onClick={this.props.toggle}>Cancel</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupToggleStatus;