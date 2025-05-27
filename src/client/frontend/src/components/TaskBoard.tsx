import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { 
  Plus, 
  Calendar, 
  User, 
  Flag,
  MoreVertical,
  Search,
  Filter
} from 'lucide-react';
import { Input } from '@/components/ui/input';
import { NewTaskModal } from './NewTaskModal';
import { apiService, TaskItem, Project } from '../services/api';
import { useToast } from '@/hooks/use-toast';

export const TaskBoard = () => {
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProjectId, setSelectedProjectId] = useState<number | undefined>(undefined);
  const [loading, setLoading] = useState(true);
  const { toast } = useToast();

  const columns = [
    { id: 'ToDo', title: 'To Do', count: 0, color: 'bg-slate-100' },
    { id: 'InProgress', title: 'In Progress', count: 0, color: 'bg-blue-100' },
    { id: 'Testing', title: 'Testing', count: 0, color: 'bg-orange-100' },
    { id: 'Done', title: 'Done', count: 0, color: 'bg-green-100' }
  ];

  // Proje listesini çek
  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const projectsData = await apiService.getProjects();
        setProjects(projectsData);
      } catch (error) {
        toast({
          title: "Error",
          description: "Failed to fetch projects.",
          variant: "destructive",
        });
      }
    };
    fetchProjects();
  }, []);

  // Seçili projeye göre taskları çek
  useEffect(() => {
    fetchTasks();
    // eslint-disable-next-line
  }, [selectedProjectId]);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      const tasksData = await apiService.getTasks(selectedProjectId);
      setTasks(tasksData);
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to fetch tasks. Please try again.",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const getTasksByStatus = (status: string) => {
    return tasks.filter(task => task.status === status);
  };

  const getColumnCount = (status: string) => {
    return getTasksByStatus(status).length;
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'Critical': return 'text-red-700 bg-red-100 border-red-200';
      case 'High': return 'text-orange-700 bg-orange-100 border-orange-200';
      case 'Medium': return 'text-blue-700 bg-blue-100 border-blue-200';
      case 'Low': return 'text-slate-700 bg-slate-100 border-slate-200';
      default: return 'text-slate-700 bg-slate-100 border-slate-200';
    }
  };

  const TaskCard = ({ task }: { task: TaskItem }) => (
    <Card className="mb-3 border-0 shadow-sm hover:shadow-md transition-all duration-200 cursor-pointer group">
      <CardContent className="p-4">
        <div className="flex items-start justify-between mb-3">
          <h4 className="font-medium text-slate-900 group-hover:text-blue-600 transition-colors">
            {task.title}
          </h4>
          <Button variant="ghost" size="sm" className="opacity-0 group-hover:opacity-100 transition-opacity">
            <MoreVertical className="h-4 w-4" />
          </Button>
        </div>
        
        <p className="text-sm text-slate-600 mb-3 line-clamp-2">
          {task.description}
        </p>
        
        <div className="flex items-center justify-between mb-3">
          <Badge className={`text-xs ${getPriorityColor(task.priority)}`}>
            <Flag className="h-3 w-3 mr-1" />
            {task.priority}
          </Badge>
          <div className="text-xs text-slate-500 bg-slate-50 px-2 py-1 rounded">
            Project #{task.projectId}
          </div>
        </div>
        
        <div className="flex items-center justify-between text-xs text-slate-500">
          <div className="flex items-center space-x-1">
            <User className="h-3 w-3" />
            <span>User #{task.assignedToUserId || 'Unassigned'}</span>
          </div>
          {task.dueDate && (
            <div className="flex items-center space-x-1">
              <Calendar className="h-3 w-3" />
              <span>{new Date(task.dueDate).toLocaleDateString()}</span>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );

  const handleTaskCreated = async (taskData: any) => {
    try {
      console.log('Creating task:', taskData);
      const newTask = await apiService.createTask(taskData);
      setTasks(prev => [...prev, newTask]);
      toast({
        title: "Task Created",
        description: `Task "${taskData.title}" has been created successfully.`,
      });
    } catch (error) {
      console.error('Error creating task:', error);
      toast({
        title: "Error",
        description: "Failed to create task. Please try again.",
        variant: "destructive",
      });
    }
  };

  if (loading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold text-slate-900">Task Board</h1>
            <p className="text-slate-600 mt-1">Loading tasks...</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-slate-900">Task Board</h1>
          <p className="text-slate-600 mt-1">Manage tasks with Kanban board</p>
        </div>
        <NewTaskModal onTaskCreated={handleTaskCreated} />
      </div>

      {/* Proje seçici */}
      <div className="flex items-center space-x-4 mb-4">
        <label className="text-sm text-slate-700">Proje:</label>
        <select
          className="border rounded px-2 py-1"
          value={selectedProjectId ?? ''}
          onChange={e => setSelectedProjectId(e.target.value ? Number(e.target.value) : undefined)}
        >
          <option value="">Tüm Projeler</option>
          {projects.map(project => (
            <option key={project.id} value={project.id}>{project.name}</option>
          ))}
        </select>
      </div>

      {/* Filters */}
      <div className="flex items-center space-x-4">
        <div className="flex-1 max-w-md">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-slate-400" />
            <Input 
              placeholder="Search tasks..." 
              className="pl-10 bg-white border-slate-200"
            />
          </div>
        </div>
        <Button variant="outline" className="border-slate-200">
          <Filter className="h-4 w-4 mr-2" />
          Filter
        </Button>
      </div>

      {/* Kanban Board */}
      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        {columns.map((column) => (
          <div key={column.id} className="space-y-4">
            <div className={`${column.color} rounded-lg p-4`}>
              <div className="flex items-center justify-between">
                <h3 className="font-semibold text-slate-900">{column.title}</h3>
                <span className="bg-white text-slate-600 text-sm px-2 py-1 rounded">
                  {getColumnCount(column.id)}
                </span>
              </div>
            </div>
            
            <div className="space-y-3">
              {getTasksByStatus(column.id).map((task) => (
                <TaskCard key={task.id} task={task} />
              ))}
              
              <div className="w-full p-4 border-2 border-dashed border-slate-200 rounded-lg text-slate-400 hover:text-slate-600 hover:border-slate-300 transition-colors flex items-center justify-center">
                <NewTaskModal onTaskCreated={handleTaskCreated}>
                  <div className="flex items-center space-x-2">
                    <Plus className="h-4 w-4" />
                    <span>Add Task</span>
                  </div>
                </NewTaskModal>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};
