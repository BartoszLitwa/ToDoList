import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupEditTaskList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskListTitle: this.props.taskList.title,
            taskListDescription: this.props.taskList.description,
            taskListDueDate: this.props.taskList.dueDate,
            taskListCompleted: this.props.taskList.isCompleted
        }
    }

    currentDate() {
        const d = new Date();
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
            taskListTitle: "",
            taskListDescription: "",
            taskListCompleted: false,
            taskListDueDate: this.currentDate(new Date()),
        });
    }

    render() { 
        const handleSave = async () => {
            console.log(this.props.taskList)
            let taskObj = {
                id: this.props.taskList.id,
                title: this.state.taskListTitle,
                description: this.state.taskListDescription,
                isCompleted: this.state.taskListCompleted,
                dueDate: new Date(this.state.taskListDueDate),
                createDate: this.props.taskList.createDate
            };

            await this.props.handleTask(taskObj);
    
            this.props.toggle();
        }
    
        const handleChange = (e) => {
            const {name, value, checked} = e.target;
            if(name === "taskListTitle") {
                this.setState({taskListTitle: value});
            } else if(name === "taskListDescription") {
                this.setState({taskListDescription: value});
            } else if(name === "taskListDueDate") {
                this.setState({taskListDueDate: value});
            } else if(name === "taskListCompleted") {
                this.setState({taskListCompleted: checked});
            } 
        }

        return (
            <Modal isOpen={this.props.modal} toggle={this.props.toggle}>
                <ModalHeader toggle={this.props.toggle}>Edit Task List</ModalHeader>
                <ModalBody>
                    <form>
                        <div className="form-group">
                            <label>Task List Name</label>
                            <input type="text" className="form-control" name="taskListTitle"
                             value={this.state.taskListTitle} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Description</label>
                            <textarea type="text" rows="5" className='form-control' name="taskListDescription"
                            value={this.state.taskListDescription} onChange={handleChange}/>
                        </div>
                        <div className="form-group">
                            <label>Due Date</label>
                            <input type="datetime-local" className="form-control" name="taskListDueDate"
                            value={this.state.taskListDueDate} onChange={handleChange}/>
                        </div>
                        <div className="form-group" style={{"alignItems": "stretch"}}>
                            <label>Task Completed</label>
                            <input type="checkbox" className='form-checkbox' name="taskListCompleted"
                            defaultChecked={this.state.taskListCompleted} onChange={handleChange}/>
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
 
export default PopupEditTaskList;