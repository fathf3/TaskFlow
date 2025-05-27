import React, { useEffect, useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Progress } from '@/components/ui/progress';
import { 
  FolderOpen, 
  CheckSquare, 
  Clock, 
  Users,
  TrendingUp,
  Calendar
} from 'lucide-react';
import { apiService, Project, User } from '../services/api';
import { NewProjectModal } from './NewProjectModal';
import { InviteMemberModal } from './InviteMemberModal';
import { NewTaskModal } from './NewTaskModal';
import { useToast } from '@/hooks/use-toast';

export const Dashboard = () => {
  const [recentProjects, setRecentProjects] = useState<Project[]>([]);
  const [projects, setProjects] = useState<Project[]>([]);
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const projects = await apiService.getProjects();
        setProjects(projects);
        setRecentProjects(projects.slice(0, 3));
      } catch (error) {
        // Hata yönetimi ekleyebilirsiniz
      }
    };
    const fetchUsers = async () => {
      try {
        const users = await apiService.getUsers();
        setUsers(users);
      } catch (error) {
        // Hata yönetimi ekleyebilirsiniz
      }
    };
    fetchProjects();
    fetchUsers();
  }, []);

  // Aktif projeleri filtrele
  const activeProjectsCount = projects.filter(p => p.status === 'Active').length;

  const stats = [
    { label: 'Active Projects', value: activeProjectsCount, icon: FolderOpen, color: 'from-blue-500 to-blue-600' },
    { label: 'Completed Tasks', value: 147, icon: CheckSquare, color: 'from-green-500 to-green-600' },
    { label: 'Pending Tasks', value: 23, icon: Clock, color: 'from-orange-500 to-orange-600' },
    { label: 'Team Members', value: users.length, icon: Users, color: 'from-purple-500 to-purple-600' },
  ];

  const handleProjectCreated = (newProject) => {
    // Projeyi oluşturduktan sonra yapılacaklar
  };

  const handleTaskCreated = (newTask) => {
    // Görevi oluşturduktan sonra yapılacaklar
  };

  const handleMemberInvited = (newMember) => {
    // Üye davet edildikten sonra yapılacaklar
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-slate-900">Dashboard</h1>
          <p className="text-slate-600 mt-1">Welcome back! Here's what's happening with your projects.</p>
        </div>
        <div className="flex items-center space-x-2 text-sm text-slate-500">
          <Calendar className="h-4 w-4" />
          <span>{new Date().toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}</span>
        </div>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, index) => (
          <Card key={index} className="relative overflow-hidden border-0 shadow-lg">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm font-medium text-slate-600">{stat.label}</p>
                  <p className="text-3xl font-bold text-slate-900 mt-2">{stat.value}</p>
                </div>
                <div className={`w-12 h-12 rounded-lg bg-gradient-to-r ${stat.color} flex items-center justify-center`}>
                  <stat.icon className="h-6 w-6 text-white" />
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Recent Projects */}
        <Card className="border-0 shadow-lg">
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <TrendingUp className="h-5 w-5 text-blue-600" />
              <span>Recent Projects</span>
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {recentProjects.map((project, index) => (
                <div key={index} className="p-4 rounded-lg bg-slate-50 hover:bg-slate-100 transition-colors cursor-pointer">
                  <div className="flex items-center justify-between mb-2">
                    <h4 className="font-medium text-slate-900">{project.name}</h4>
                    <span className={`px-2 py-1 rounded-full text-xs font-medium ${
                      project.status === 'In Progress' ? 'bg-blue-100 text-blue-700' :
                      project.status === 'Testing' ? 'bg-orange-100 text-orange-700' :
                      'bg-slate-100 text-slate-700'
                    }`}>
                      {project.status}
                    </span>
                  </div>
                  <div className="space-y-2">
                    <div className="flex items-center justify-between text-sm text-slate-600">
                      <span>Progress</span>
                      <span>{project.status}%</span>
                    </div>
                    <Progress value={project.id} className="h-2" />
                    <div className="text-xs text-slate-500">
                      Due: {new Date(project.id).toLocaleDateString()}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Quick Actions */}
        <Card className="border-0 shadow-lg">
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              <NewProjectModal onProjectCreated={handleProjectCreated}>
                <div className="p-4 rounded-lg bg-gradient-to-r from-blue-50 to-blue-100 hover:from-blue-100 hover:to-blue-200 transition-all border border-blue-200 flex flex-col items-center cursor-pointer">
                  <FolderOpen className="h-8 w-8 text-blue-600 mb-2" />
                  <div className="text-sm font-medium text-blue-700">New Project</div>
                </div>
              </NewProjectModal>
              <NewTaskModal onTaskCreated={handleTaskCreated}>
                <div className="p-4 rounded-lg bg-gradient-to-r from-green-50 to-green-100 hover:from-green-100 hover:to-green-200 transition-all border border-green-200 flex flex-col items-center cursor-pointer">
                  <CheckSquare className="h-8 w-8 text-green-600 mb-2" />
                  <div className="text-sm font-medium text-green-700">Add Task</div>
                </div>
              </NewTaskModal>
              <InviteMemberModal onMemberInvited={handleMemberInvited}>
                <div className="p-4 rounded-lg bg-gradient-to-r from-purple-50 to-purple-100 hover:from-purple-100 hover:to-purple-200 transition-all border border-purple-200 flex flex-col items-center cursor-pointer">
                  <Users className="h-8 w-8 text-purple-600 mb-2" />
                  <div className="text-sm font-medium text-purple-700">Invite Member</div>
                </div>
              </InviteMemberModal>
              <button className="p-4 rounded-lg bg-gradient-to-r from-orange-50 to-orange-100 hover:from-orange-100 hover:to-orange-200 transition-all border border-orange-200 flex flex-col items-center cursor-pointer">
                <TrendingUp className="h-8 w-8 text-orange-600 mb-2" />
                <div className="text-sm font-medium text-orange-700">View Reports</div>
              </button>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
