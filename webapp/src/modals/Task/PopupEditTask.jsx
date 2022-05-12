import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupEditTask extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskTitle: this.props.task.title,
            taskDescription: this.props.task.description,
            taskCompletedDate: this.props.task.completedDate,
            taskDueDate: this.props.task.dueDate,
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
            taskTitle: "",
            taskDescription: "",
            taskDueDate: this.currentDate(),
            taskCompleted: false,
        });
    }

    populate() {
        this.setState({
            taskTitle: this.props.task.title,
            taskDescription: this.props.task.description,
            taskCompletedDate: this.props.task.completedDate,
            taskDueDate: this.props.task.dueDate,
            taskCompleted: this.props.task.isCompleted
        });
    }

    render() { 
        const task = this.props.task;

        const handleSave = async () => {
            let taskObj = {
                id: task.id,
                title: this.state.taskTitle,
                description: this.state.taskDescription,
                isCompleted: this.state.taskCompleted,
                dueDate: this.currentDate(new Date(this.state.taskDueDate)),
                completedDate: this.currentDate(new Date(this.state.taskCompletedDate)),
            };

            await this.props.handleTask(taskObj);
    
            this.props.toggle();
        }
    
        const handleChange = (e) => {
            const {name, value, checked} = e.target;
            if(name === "taskTitle") {
                this.setState({taskTitle: value});
            } else if(name === "taskDescription") {
                this.setState({taskDescription: value});
            } else if(name === "taskDueDate") {
                this.setState({taskDueDate: value});
            } else if(name === "taskCompletedDate") {
                this.setState({taskCompletedDate: value});
            } else if(name === "taskCompleted") {
                this.setState({taskCompleted: checked});
            } 
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>{"Edit " + task.title + " Task"}</ModalHeader>
                <ModalBody>
                    <form>
                        <div className="form-group">
                            <label>Task Name</label>
                            <input type="text" className="form-control" name="taskTitle"
                             value={this.state.taskTitle} onChange={handleChange}/>
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
                            <label>Completed Date</label>
                            <input type="datetime-local" className="form-control" name="taskCompletedDate"
                            value={this.state.taskCompletedDate} onChange={handleChange}/>
                        </div>
                    </form>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-primary' onClick={async () => await handleSave()}>Update</button>
                    <button className='btn btn-secondary' onClick={this.props.toggle}>Cancel</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupEditTask;