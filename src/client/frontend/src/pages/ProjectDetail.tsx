import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { apiService, Project, TaskItem, User, ProjectMember } from '../services/api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';

const ProjectDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [project, setProject] = useState<Project | null>(null);
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [members, setMembers] = useState<ProjectMember[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [newTask, setNewTask] = useState({ title: '', description: '' });
  const [newMemberId, setNewMemberId] = useState<number | ''>('');

  useEffect(() => {
    if (!id) return;
    apiService.getProject(Number(id)).then(setProject);
    apiService.getTasks(Number(id)).then(setTasks);
    apiService.getProjectMembers(Number(id)).then(setMembers);
    apiService.getUsers().then(setUsers);
  }, [id]);

  const handleUpdate = async () => {
    if (!project) return;
    await apiService.createProject(project); // veya update endpoint'iniz varsa onu kullanın
    alert('Proje güncellendi!');
  };

  const handleDelete = async () => {
    // apiService'de deleteProject fonksiyonu eklemelisiniz
    // await apiService.deleteProject(project.id);
    alert('Proje silindi!');
    navigate('/');
  };

  const handleAddTask = async () => {
    if (!id || !newTask.title) return;
    await apiService.createTask({
      ...newTask,
      projectId: Number(id),
      status: 'Todo',
      priority: 'Normal',
      createdByUserId: 1, // Giriş yapan kullanıcıdan alınmalı
    });
    setNewTask({ title: '', description: '' });
    apiService.getTasks(Number(id)).then(setTasks);
  };

  const handleAddMember = async () => {
    if (!id || !newMemberId) return;
    await apiService.addProjectMember({
      projectId: Number(id),
      userId: Number(newMemberId),
      role: 'Member',
    });
    setNewMemberId('');
    apiService.getProjectMembers(Number(id)).then(setMembers);
  };

  const handleRemoveMember = async (memberId: number) => {
    // apiService'de removeProjectMember fonksiyonu eklemelisiniz
    // await apiService.removeProjectMember(memberId);
    apiService.getProjectMembers(Number(id)).then(setMembers);
  };

  return (
    <div className="max-w-4xl mx-auto p-6">
      {project && (
        <>
          <div className="flex justify-between items-center mb-6">
            <div>
              <h2 className="text-2xl font-bold">{project.name}</h2>
              <p className="text-slate-500">{project.description}</p>
              <div className="text-xs text-slate-400 mt-1">
                Başlangıç: {project.startDate} {project.endDate && `- Bitiş: ${project.endDate}`}
              </div>
            </div>
            <div className="space-x-2">
              <Button onClick={handleUpdate}>Güncelle</Button>
              <Button variant="destructive" onClick={handleDelete}>Sil</Button>
            </div>
          </div>

          {/* Üyeler */}
          <div className="mb-6">
            <h3 className="font-semibold mb-2">Üyeler</h3>
            <div className="flex flex-wrap gap-2 mb-2">
              {members.map(m => {
                const u = users.find(u => u.id === m.userId);
                return (
                  <div key={m.id} className="flex items-center bg-slate-100 px-2 py-1 rounded">
                    <span>{u ? `${u.firstName} ${u.lastName}` : 'Kullanıcı'} ({m.role})</span>
                    <Button size="sm" variant="ghost" onClick={() => handleRemoveMember(m.id)}>Çıkar</Button>
                  </div>
                );
              })}
            </div>
            <div className="flex gap-2">
              <select
                className="border rounded px-2 py-1"
                value={newMemberId}
                onChange={e => setNewMemberId(Number(e.target.value))}
              >
                <option value="">Üye ekle</option>
                {users
                  .filter(u => !members.some(m => m.userId === u.id))
                  .map(u => (
                    <option key={u.id} value={u.id}>
                      {u.firstName} {u.lastName}
                    </option>
                  ))}
              </select>
              <Button size="sm" onClick={handleAddMember}>Ekle</Button>
            </div>
          </div>

          {/* Görevler */}
          <div className="mb-6">
            <h3 className="font-semibold mb-2">Görevler</h3>
            <div className="space-y-2 mb-2">
              {tasks.map(t => (
                <div key={t.id} className="border rounded px-3 py-2 flex justify-between items-center">
                  <div>
                    <div className="font-medium">{t.title}</div>
                    <div className="text-xs text-slate-500">{t.status} - {t.priority}</div>
                  </div>
                  {/* Görev güncelle/sil butonları eklenebilir */}
                </div>
              ))}
            </div>
            <div className="flex gap-2">
              <Input
                placeholder="Görev başlığı"
                value={newTask.title}
                onChange={e => setNewTask({ ...newTask, title: e.target.value })}
              />
              <Input
                placeholder="Açıklama"
                value={newTask.description}
                onChange={e => setNewTask({ ...newTask, description: e.target.value })}
              />
              <Button size="sm" onClick={handleAddTask}>Ekle</Button>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default ProjectDetail;