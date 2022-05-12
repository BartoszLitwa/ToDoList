import React, { Component } from 'react';
import PopupCreateTask from '../../modals/Task/PopupCreateTask';
import PopupDeleteTaskList from '../../modals/TaskList/PopupDeleteTaskList';
import PopupEditTaskList from '../../modals/TaskList/PopupEditTaskList';
import PopupQuickTaskView from '../../modals/TaskList/PopupQuickTaskView';
import { WithRouter } from '../../WithRouter';

class TaskListCard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modalEdit: false,
            modalDelete: false,
            modalShow: false,
            modalAdd: false,
            modalQuickView: false
        }
    }

    render() {
        const taskList = this.props.taskListObj;

        const toLocalDate = (date) => {
            const d = new Date(date);
            return d.toLocaleString();
        }

        const handleEdit = async (obj) => {
            await this.props.callEndpoint("taskList/update", "PUT", 
            {
                TaskListId: obj.id,
                Title: obj.title,
                Description: obj.description,
                IsCompleted: obj.isCompleted,
                DueDate: obj.dueDate,
                Tasks: obj.tasks
            },
            (res) => {
                this.props.updateTaskListObj(res);
            });
        }

        const handleDelete = async () => {
            console.log(taskList)
            await this.props.callEndpoint('taskList/delete', 'DELETE', 
            {
                TaskListId: taskList.id
            },
            async (res) => {
                this.props.deleteTaskListObj(res);
            });
        }

        const handleShow = async () => {
            this.props.navigate('/todolist', {state: {taskList: taskList}});
        }

        const addTask = async (task) => {
            await this.props.callEndpoint("task/create", "POST", 
            {
                TaskListId: taskList.id,
                DueDate: task.DueDate,
                CompletedDate: task.CompletedDate,
                Title: task.Title,
                Description: task.Description,
                IsCompleted: task.IsCompleted 
            },
            (res) => {
                this.props.navigate('/todolist', {state: {taskList: taskList}});
            });
        }

        const renderTasks = (s) => {
            const data = [].concat(taskList.tasks)
                .sort((a,b) => a.title < b.title ? -1 : 1)
                .map((obj, index) => {
                    <PopupQuickTaskView task={obj} modal={this.state.modalQuickView[index]} toggle={() => this.setState({modalQuickView: !this.state.modalQuickView})}/>
    
                    return <h6 className='text-white' key={obj.id} onClick={() => {
                        this.setState({modalQuickView: true});

                    }} style={{'cursor': 'pointer'}}
                    >{(index + 1) + ". " + obj.title}</h6>
                });

            return data;
        }
        
        return ( 
            <div className='card-wrapper'>
                <div className='card-top'></div>
                <div className='task-holder text-white'>
                    <h4 className='text-white'>{taskList.title}</h4>
                    <h6 className='text-white'>{taskList.description}</h6>
                    <h6 className='text-white'>{"Due: " + toLocalDate(taskList.dueDate)}</h6>
                    <h6 className='text-white'>{"Created: " + toLocalDate(taskList.createDate)}</h6>
                    <hr />
                    <h5 className='text-white'> {"Tasks - " + (taskList.tasks === undefined ? 0 : taskList.tasks.length)} </h5>
                    <div className='m-3 task-list-content'>
                        {renderTasks(1)}
                    </div>
                    

                    <div style={{"position": "absolute", "bottom": "-20px", "left": "10px"}}>
                        <button className='btn' style={{"color": "green"}} onClick={() => this.setState({modalEdit: true})}>Edit</button>
                        <button className='btn' style={{"color": "red"}} onClick={() => this.setState({modalDelete: true})}>Delete</button>
                        <button className='btn' style={{"color": "white"}} onClick={async () => await handleShow()}>Show Tasks</button>
                        <button className='btn' style={{"color": "white"}} onClick={() => this.setState({modalAdd: true})}>Add Task</button>
                    </div>
                </div>

                <PopupEditTaskList taskList={taskList} handleTask={handleEdit} modal={this.state.modalEdit} toggle={() => this.setState({modalEdit: !this.state.modalEdit})} />
                <PopupDeleteTaskList taskList={taskList} handleTask={handleDelete} modal={this.state.modalDelete} toggle={() => this.setState({modalDelete: !this.state.modalDelete})} />
                <PopupCreateTask taskList={taskList} handleTask={addTask} modal={this.state.modalAdd} toggle={() => this.setState({modalAdd: !this.state.modalAdd})} />
            </div>
        );
    }
}
 
export default WithRouter(TaskListCard);