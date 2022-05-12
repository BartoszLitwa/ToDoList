import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupCreateTask extends Component {
    constructor(props) {
        super(props);
        let date = this.currentDate(new Date());

        this.state = {
            modal: false,
            taskName: "",
            taskDescription: "",
            taskDueDate: date,
            taskCompletedDate: date,
            taskCompleted: false
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
            taskName: "",
            taskDescription: "",
            taskCompleted: false,
        });
    }

    render() { 
        const handleSave = async () => {
            let taskObj = {};
            taskObj["Title"] = this.state.taskName;
            taskObj["Description"] = this.state.taskDescription;
            taskObj["DueDate"] = this.currentDate(new Date(this.state.taskDueDate));
            taskObj["IsCompleted"] = this.state.taskCompleted;
            taskObj["CompletedDate"] = this.currentDate(new Date(this.state.taskCompletedDate));
    
            await this.props.handleTask(taskObj);
            this.clearData();
            this.props.toggle();
        }

        const toggle = () => {
            this.props.toggle();
            this.clearData();
        }
    
        const handleChange = (e) => {
            const {name, value, checked} = e.target;
            if(name === "taskName") {
                this.setState({taskName: value});
            } else if(name === "taskDescription") {
                this.setState({taskDescription: value});
            } else if(name === "taskDueDate") {
                this.setState({taskDueDate: value});
            } else if(name === "taskCompleted") {
                this.setState({taskCompleted: checked});
            } else if(name === "taskCompletedDate") {
                this.setState({taskCompletedDate: value});
            }
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>Create Task</ModalHeader>
                <ModalBody>
                    <form>
                        <div className="form-group">
                            <label>Task Title</label>
                            <input type="text" className="form-control" name="taskName"
                             value={this.state.taskName} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Description</label>
                            <textarea type="text" rows="5" className='form-control' name="taskDescription"
                            value={this.state.taskDescription} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Due Date</label>
                            <input type="datetime-local" className="form-control" name="taskDueDate"
                            value={this.state.taskDueDate} onChange={handleChange}/>
                        </div>
                        <div className="form-group" style={{"alignItems": "stretch"}}>
                            <label>Task Completed</label>
                            <input type="checkbox" className='form-checkbox' name="taskCompleted"
                            defaultChecked={this.state.taskCompleted} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Complete Date</label>
                            <input type="datetime-local" className="form-control" name="taskCompletedDate"
                            value={this.state.taskCompletedDate} onChange={handleChange}/>
                        </div>
                    </form>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-primary' onClick={async () => await handleSave()}>Create</button>
                    <button className='btn btn-secondary' onClick={toggle}>Cancel</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupCreateTask;