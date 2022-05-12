import React, { Component } from 'react';
import {Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class PopupCreateTaskList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            taskListTitle: "",
            taskListDescription: "",
            taskListDueDate: this.currentDate(new Date()),
            taskListCompleted: false
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
            taskListTitle: "",
            taskListDescription: "",
            taskListCompleted: false
        });
    }

    render() { 
        const handleSave = async () => {
            let taskObj = {};
            taskObj["Title"] = this.state.taskListTitle;
            taskObj["Description"] = this.state.taskListDescription;
            taskObj["IsCompleted"] = this.state.taskListCompleted;
            taskObj["DueDate"] = this.currentDate(new Date(this.state.taskListDueDate));

            await this.props.callEndpoint('tasklist/create', "POST",
            taskObj,
            async (res) => {
                console.log(res)
                if(res.success === true){
                    this.props.handleTask(res);
                } else {
                    this.setState({errors: res.errors});
                }
                this.props.toggle();
            });

            this.clearData();
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
                <ModalHeader toggle={this.props.toggle}>Create Task List</ModalHeader>
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
                            <label>Task List Completed</label>
                            <input type="checkbox" className='form-checkbox' name="taskListCompleted"
                            defaultChecked={this.state.taskListCompleted} onChange={handleChange}/>
                        </div>
                    </form>
                </ModalBody>
                <ModalFooter>
                    <button className='btn btn-primary' onClick={async () => await handleSave()}>Create</button>
                    <button className='btn btn-secondary' onClick={this.props.toggle}>Cancel</button>
                </ModalFooter>
            </Modal>
        );
    }
}
 
export default PopupCreateTaskList;